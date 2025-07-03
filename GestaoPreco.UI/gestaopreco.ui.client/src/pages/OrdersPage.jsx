import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getOrders, createOrderWithItems, deleteOrder } from '../api/orderService';
import { getCustomers } from '../api/customerService';
import { getProducts } from '../api/productService';

export default function OrdersPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error, refetch } = useQuery({
    queryKey: ['orders'],
    queryFn: getOrders,
    select: (res) => res.data,
    onSuccess: (data) => {
      console.log('Dados carregados com sucesso:', data);
    },
    onError: (error) => {
      console.error('Erro ao carregar pedidos:', error);
    }
  });

  // Buscar clientes e produtos para os selects
  const { data: customers } = useQuery({
    queryKey: ['customers'],
    queryFn: getCustomers,
    select: (res) => res.data
  });

  const { data: products } = useQuery({
    queryKey: ['products'],
    queryFn: getProducts,
    select: (res) => res.data
  });

  const [form, setForm] = useState({ 
    customerId: '', 
    date: new Date().toISOString().split('T')[0], 
    status: 'Pendente',
    items: []
  });

  const [newItem, setNewItem] = useState({
    productId: '',
    quantity: 1,
    unitPrice: 0
  });

  const createMutation = useMutation({
    mutationFn: createOrderWithItems,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
      setForm({ 
        customerId: '', 
        date: new Date().toISOString().split('T')[0], 
        status: 'Pendente',
        items: []
      });
      alert('Pedido criado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao criar pedido: ' + error.message);
    }
  });
  
  const deleteMutation = useMutation({
    mutationFn: deleteOrder,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orders'] });
      alert('✅ Pedido removido com sucesso!');
    },
    onError: (error) => {
      console.error('Erro detalhado do delete:', error);
      const errorMessage = error.response?.data || error.message || 'Erro desconhecido';
      alert(`❌ Erro ao remover pedido: ${errorMessage}`);
    }
  });

  const handleSubmit = e => {
    e.preventDefault();
    if (form.items.length === 0) {
      alert('Adicione pelo menos um item ao pedido');
      return;
    }
    
    // Calcular o total do pedido
    const totalAmount = form.items.reduce((total, item) => total + (item.quantity * item.unitPrice), 0);
    
    const orderData = {
      customerId: form.customerId,
      date: new Date(form.date).toISOString(),
      status: convertStatusToEnum(form.status),
      totalAmount: totalAmount,
      items: form.items.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        unitPrice: item.unitPrice
      }))
    };
    
    console.log('Enviando dados do pedido:', JSON.stringify(orderData, null, 2));
    createMutation.mutate(orderData);
  };

  const addItem = () => {
    if (!newItem.productId || newItem.quantity <= 0 || newItem.unitPrice <= 0) {
      alert('Preencha todos os campos do item');
      return;
    }

    const selectedProduct = products?.find(p => p.id === newItem.productId);
    if (!selectedProduct) {
      alert('Produto não encontrado');
      return;
    }

    const item = {
      id: Date.now(), // ID temporário
      productId: newItem.productId,
      productName: selectedProduct.name,
      quantity: parseInt(newItem.quantity),
      unitPrice: parseFloat(newItem.unitPrice),
      totalPrice: newItem.quantity * newItem.unitPrice
    };

    setForm(prev => ({
      ...prev,
      items: [...prev.items, item]
    }));

    setNewItem({
      productId: '',
      quantity: 1,
      unitPrice: 0
    });
  };

  const removeItem = (index) => {
    setForm(prev => ({
      ...prev,
      items: prev.items.filter((_, i) => i !== index)
    }));
  };

  const handleDelete = (order) => {
    if (window.confirm(`Tem certeza que deseja remover o pedido ID: ${order.id}?`)) {
      deleteMutation.mutate(order.id);
    }
  };

  const handleTestRequest = async () => {
    console.log('Testando requisição manual...');
    try {
      const response = await getOrders();
      console.log('Teste manual bem-sucedido:', response);
      alert('Requisição funcionou! Verifique o console.');
    } catch (error) {
      console.error('Teste manual falhou:', error);
      alert('Erro na requisição: ' + error.message);
    }
  };

  console.log('Estado atual:', { isLoading, error, data });

  if (isLoading) return <div className="loading">⏳ Carregando pedidos...</div>;
  if (error) return (
    <div className="error">
      ❌ Erro ao carregar pedidos: {error.message}
      <div className="d-flex gap-10 mt-20">
        <button className="btn btn-secondary" onClick={handleTestRequest}>
          🧪 Testar Requisição Manual
        </button>
        <button className="btn btn-primary" onClick={() => refetch()}>
          🔄 Tentar Novamente
        </button>
      </div>
    </div>
  );

  const getStatusBadge = (status) => {
    const statusMap = {
      'Pendente': 'badge-pending',
      'Processando': 'badge-processing',
      'Concluido': 'badge-completed',
      'Cancelado': 'badge-cancelled'
    };
    return `badge ${statusMap[status] || 'badge-pending'}`;
  };

  // Função para converter status do frontend para o enum do backend
  const convertStatusToEnum = (status) => {
    const statusMap = {
      'Pendente': 0, // OrderStatus.Pendente
      'Processando': 1, // OrderStatus.Processando
      'Concluido': 2, // OrderStatus.Concluido
      'Cancelado': 3 // OrderStatus.Cancelado
    };
    return statusMap[status] || 0;
  };

  return (
    <div className="fade-in">
      <div className="card">
        <div className="d-flex justify-between align-center mb-20">
          <h2>📋 Gerenciamento de Pedidos</h2>
          <div className="d-flex gap-10">
            <div className="badge badge-completed">
              Total: {data ? data.length : 0} pedidos
            </div>
            <button 
              className="btn btn-secondary"
              onClick={() => {
                if (!customers || customers.length === 0 || !products || products.length === 0) {
                  alert('❌ É necessário ter pelo menos um cliente e um produto cadastrado para testar');
                  return;
                }
                
                const testData = {
                  customerId: customers[0].id,
                  date: new Date().toISOString(),
                  status: convertStatusToEnum('Pendente'),
                  totalAmount: 100.00,
                  items: [
                    {
                      productId: products[0].id,
                      quantity: 1,
                      unitPrice: 100.00
                    }
                  ]
                };
                console.log('Testando create com dados:', JSON.stringify(testData, null, 2));
                createMutation.mutate(testData);
              }}
              disabled={createMutation.isPending || !customers || !products}
            >
              🧪 Testar Create
            </button>
            <button 
              className="btn btn-secondary"
              onClick={async () => {
                try {
                  console.log('Testando endpoint /api/Order/with-items');
                  const response = await getOrdersWithItems();
                  console.log('Resposta getOrdersWithItems:', response);
                  alert('✅ Endpoint /api/Order/with-items funcionando! Verifique o console.');
                } catch (error) {
                  console.error('Erro no endpoint /api/Order/with-items:', error);
                  alert('❌ Erro no endpoint /api/Order/with-items: ' + error.message);
                }
              }}
            >
              🧪 Testar /with-items
            </button>
          </div>
        </div>
        
        {/* Formulário de criação de pedido */}
        <div className="card">
          <h3>➕ Criar Novo Pedido</h3>
          <form onSubmit={handleSubmit}>
            <div className="grid grid-3">
              <div className="form-group">
                <label>Cliente</label>
                <select 
                  className="form-control"
                  value={form.customerId} 
                  onChange={e => setForm(f => ({ ...f, customerId: e.target.value }))}
                  required
                >
                  <option value="">Selecione um cliente</option>
                  {customers?.map(c => (
                    <option key={c.id} value={c.id}>{c.name}</option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Data do Pedido</label>
                <input
                  className="form-control"
                  type="date"
                  value={form.date}
                  onChange={e => setForm(f => ({ ...f, date: e.target.value }))}
                  required
                />
              </div>

              <div className="form-group">
                <label>Status</label>
                <select 
                  className="form-control"
                  value={form.status} 
                  onChange={e => setForm(f => ({ ...f, status: e.target.value }))}
                  required
                >
                  <option value="Pendente">Pendente</option>
                  <option value="Processando">Processando</option>
                  <option value="Concluido">Concluído</option>
                  <option value="Cancelado">Cancelado</option>
                </select>
              </div>
            </div>

            {/* Adicionar itens */}
            <div className="card">
              <h4>🛒 Adicionar Item</h4>
              <div className="grid grid-3">
                <div className="form-group">
                  <label>Produto</label>
                  <select 
                    className="form-control"
                    value={newItem.productId} 
                    onChange={e => setNewItem(f => ({ ...f, productId: e.target.value }))}
                  >
                    <option value="">Selecione um produto</option>
                    {products?.map(p => (
                      <option key={p.id} value={p.id}>{p.name} - R$ {p.price}</option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label>Quantidade</label>
                  <input
                    className="form-control"
                    type="number"
                    min="1"
                    value={newItem.quantity}
                    onChange={e => setNewItem(f => ({ ...f, quantity: e.target.value }))}
                  />
                </div>

                <div className="form-group">
                  <label>Preço Unitário (R$)</label>
                  <input
                    className="form-control"
                    type="number"
                    step="0.01"
                    min="0"
                    value={newItem.unitPrice}
                    onChange={e => setNewItem(f => ({ ...f, unitPrice: e.target.value }))}
                  />
                </div>
              </div>
              <button type="button" className="btn btn-secondary" onClick={addItem}>
                ➕ Adicionar Item
              </button>
            </div>

            {/* Lista de itens do pedido */}
            {form.items.length > 0 && (
              <div className="card">
                <h4>📦 Itens do Pedido</h4>
                <div className="table-container">
                  <table className="table">
                    <thead>
                      <tr>
                        <th>Produto</th>
                        <th>Quantidade</th>
                        <th>Preço Unit.</th>
                        <th>Total</th>
                        <th>Ação</th>
                      </tr>
                    </thead>
                    <tbody>
                      {form.items.map((item, index) => (
                        <tr key={index}>
                          <td><strong>{item.productName}</strong></td>
                          <td>{item.quantity}</td>
                          <td>R$ {item.unitPrice}</td>
                          <td>
                            <span className="badge badge-completed">
                              R$ {item.totalPrice}
                            </span>
                          </td>
                          <td>
                            <button 
                              type="button" 
                              className="btn btn-danger"
                              onClick={() => removeItem(index)}
                            >
                              🗑️ Remover
                            </button>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
                <div className="text-center mt-20">
                  <h4 className="badge badge-processing">
                    Total do Pedido: R$ {form.items.reduce((total, item) => total + item.totalPrice, 0).toFixed(2)}
                  </h4>
                </div>
              </div>
            )}

            <button 
              type="submit" 
              className="btn btn-primary"
              disabled={createMutation.isPending}
            >
              {createMutation.isPending ? '⏳ Criando...' : '✅ Criar Pedido'}
            </button>
          </form>
        </div>

        {/* Lista de pedidos existentes */}
        <div className="card">
          <h3>📋 Pedidos Existentes</h3>
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Cliente</th>
                  <th>Data</th>
                  <th>Status</th>
                  <th>Total</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {data && data.length > 0 ? (
                  data.map(o => (
                    <tr key={o.id}>
                      <td><strong>#{o.id.slice(0, 8)}</strong></td>
                      <td>{o.customerId}</td>
                      <td>{new Date(o.orderDate).toLocaleDateString('pt-BR')}</td>
                      <td>
                        <span className={getStatusBadge(o.status)}>
                          {o.status}
                        </span>
                      </td>
                      <td>
                        <span className="badge badge-completed">
                          R$ {parseFloat(o.totalAmount || 0).toFixed(2)}
                        </span>
                      </td>
                      <td>
                        <button 
                          className="btn btn-danger"
                          onClick={() => handleDelete(o)}
                          disabled={deleteMutation.isPending}
                        >
                          {deleteMutation.isPending ? '⏳ Removendo...' : '🗑️ Remover'}
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="6" className="text-center">
                      📭 Nenhum pedido encontrado
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
} 