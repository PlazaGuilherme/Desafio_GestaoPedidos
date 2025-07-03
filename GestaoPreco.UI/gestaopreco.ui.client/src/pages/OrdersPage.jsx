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
    status: 'Pending',
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
        status: 'Pending',
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
      alert('Pedido removido com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao remover pedido: ' + error.message);
    }
  });

  const handleSubmit = e => {
    e.preventDefault();
    if (form.items.length === 0) {
      alert('Adicione pelo menos um item ao pedido');
      return;
    }
    
    const orderData = {
      customerId: form.customerId,
      date: form.date,
      status: form.status,
      totalAmount: form.items.reduce((total, item) => total + (item.quantity * item.unitPrice), 0),
      items: form.items
    };
    
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

  if (isLoading) return <div>Carregando...</div>;
  if (error) return (
    <div>
      <div>Erro ao carregar pedidos: {error.message}</div>
      <button onClick={handleTestRequest}>Testar Requisição Manual</button>
      <button onClick={() => refetch()}>Tentar Novamente</button>
    </div>
  );

  return (
    <div>
      <h2>Pedidos</h2>
      <p>Total de pedidos: {data ? data.length : 0}</p>
      <button onClick={handleTestRequest} style={{ marginBottom: 10 }}>Testar Requisição</button>
      
      {/* Formulário de criação de pedido */}
      <div style={{ border: '1px solid #ccc', padding: 20, marginBottom: 20 }}>
        <h3>Criar Novo Pedido</h3>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 10 }}>
            <label>Cliente: </label>
            <select 
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

          <div style={{ marginBottom: 10 }}>
            <label>Data: </label>
            <input
              type="date"
              value={form.date}
              onChange={e => setForm(f => ({ ...f, date: e.target.value }))}
              required
            />
          </div>

          <div style={{ marginBottom: 10 }}>
            <label>Status: </label>
            <select 
              value={form.status} 
              onChange={e => setForm(f => ({ ...f, status: e.target.value }))}
              required
            >
              <option value="Pending">Pendente</option>
              <option value="Processing">Processando</option>
              <option value="Completed">Concluído</option>
              <option value="Cancelled">Cancelado</option>
            </select>
          </div>

          {/* Adicionar itens */}
          <div style={{ border: '1px solid #ddd', padding: 10, marginBottom: 10 }}>
            <h4>Adicionar Item</h4>
            <div style={{ marginBottom: 10 }}>
              <label>Produto: </label>
              <select 
                value={newItem.productId} 
                onChange={e => setNewItem(f => ({ ...f, productId: e.target.value }))}
              >
                <option value="">Selecione um produto</option>
                {products?.map(p => (
                  <option key={p.id} value={p.id}>{p.name} - R$ {p.price}</option>
                ))}
              </select>
            </div>

            <div style={{ marginBottom: 10 }}>
              <label>Quantidade: </label>
              <input
                type="number"
                min="1"
                value={newItem.quantity}
                onChange={e => setNewItem(f => ({ ...f, quantity: e.target.value }))}
              />
            </div>

            <div style={{ marginBottom: 10 }}>
              <label>Preço Unitário: </label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={newItem.unitPrice}
                onChange={e => setNewItem(f => ({ ...f, unitPrice: e.target.value }))}
              />
            </div>

            <button type="button" onClick={addItem}>Adicionar Item</button>
          </div>

          {/* Lista de itens do pedido */}
          {form.items.length > 0 && (
            <div style={{ marginBottom: 10 }}>
              <h4>Itens do Pedido</h4>
              <table border="1" cellPadding="5" style={{ width: '100%' }}>
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
                      <td>{item.productName}</td>
                      <td>{item.quantity}</td>
                      <td>R$ {item.unitPrice}</td>
                      <td>R$ {item.totalPrice}</td>
                      <td>
                        <button type="button" onClick={() => removeItem(index)}>Remover</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              <p><strong>Total do Pedido: R$ {form.items.reduce((total, item) => total + item.totalPrice, 0)}</strong></p>
            </div>
          )}

          <button type="submit" disabled={createMutation.isPending}>
            {createMutation.isPending ? 'Criando...' : 'Criar Pedido'}
          </button>
        </form>
      </div>

      {/* Lista de pedidos existentes */}
      <h3>Pedidos Existentes</h3>
      <table border="1" cellPadding="8">
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
                <td>{o.id}</td>
                <td>{o.customerId}</td>
                <td>{o.orderDate}</td>
                <td>{o.status}</td>
                <td>{o.totalAmount}</td>
                <td>
                  <button onClick={() => handleDelete(o)}>Remover</button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6">Nenhum pedido encontrado</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
} 