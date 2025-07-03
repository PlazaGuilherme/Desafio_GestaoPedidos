import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getOrderItems, createOrderItem, updateOrderItem, deleteOrderItem } from '../api/orderItemService';

export default function OrderItemsPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({
    queryKey: ['orderItems'],
    queryFn: getOrderItems,
    select: res => res.data
  });
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({ productId: '', productName: '', quantity: '', unitPrice: '', orderId: '' });

  const createMutation = useMutation({
    mutationFn: createOrderItem,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orderItems'] });
      setForm({ productId: '', productName: '', quantity: '', unitPrice: '', orderId: '' });
      alert('Item de pedido criado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao criar item de pedido: ' + error.message);
    }
  });
  
  const updateMutation = useMutation({
    mutationFn: ({ id, data }) => updateOrderItem(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orderItems'] });
      setEditing(null);
      setForm({ productId: '', productName: '', quantity: '', unitPrice: '', orderId: '' });
      alert('Item de pedido atualizado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao atualizar item de pedido: ' + error.message);
    }
  });
  
  const deleteMutation = useMutation({
    mutationFn: deleteOrderItem,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['orderItems'] });
      alert('Item de pedido removido com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao remover item de pedido: ' + error.message);
    }
  });

  const handleSubmit = e => {
    e.preventDefault();
    if (editing) {
      updateMutation.mutate({ id: editing.id, data: form });
    } else {
      createMutation.mutate(form);
    }
  };

  const handleEdit = item => {
    setEditing(item);
    setForm({
      productId: item.productId,
      productName: item.productName,
      quantity: item.quantity,
      unitPrice: item.unitPrice,
      orderId: item.orderId
    });
  };

  const handleDelete = (item) => {
    if (window.confirm(`Tem certeza que deseja remover o item "${item.productName}"?`)) {
      deleteMutation.mutate(item.id);
    }
  };

  const handleCancelEdit = () => {
    setEditing(null);
    setForm({ productId: '', productName: '', quantity: '', unitPrice: '', orderId: '' });
  };

  if (isLoading) return <div className="loading">⏳ Carregando itens de pedido...</div>;
  if (error) return <div className="error">❌ Erro ao carregar itens de pedido: {error.message}</div>;

  return (
    <div className="fade-in">
      <div className="card">
        <div className="d-flex justify-between align-center mb-20">
          <h2>🛒 Gerenciamento de Itens de Pedido</h2>
          <div className="badge badge-processing">
            Total: {data ? data.length : 0} itens
          </div>
        </div>
        
        {/* Formulário de criação/edição */}
        <div className="card">
          <h3>{editing ? '✏️ Editar Item de Pedido' : '➕ Criar Novo Item de Pedido'}</h3>
          <form onSubmit={handleSubmit}>
            <div className="grid grid-2">
              <div className="form-group">
                <label>ID do Produto</label>
                <input
                  className="form-control"
                  placeholder="Digite o ID do produto"
                  value={form.productId}
                  onChange={e => setForm(f => ({ ...f, productId: e.target.value }))}
                  required
                />
              </div>
              <div className="form-group">
                <label>Nome do Produto</label>
                <input
                  className="form-control"
                  placeholder="Digite o nome do produto"
                  value={form.productName}
                  onChange={e => setForm(f => ({ ...f, productName: e.target.value }))}
                  required
                />
              </div>
            </div>
            <div className="grid grid-3">
              <div className="form-group">
                <label>Quantidade</label>
                <input
                  className="form-control"
                  placeholder="Quantidade"
                  type="number"
                  min="1"
                  value={form.quantity}
                  onChange={e => setForm(f => ({ ...f, quantity: e.target.value }))}
                  required
                />
              </div>
              <div className="form-group">
                <label>Preço Unitário (R$)</label>
                <input
                  className="form-control"
                  placeholder="0.00"
                  type="number"
                  step="0.01"
                  min="0"
                  value={form.unitPrice}
                  onChange={e => setForm(f => ({ ...f, unitPrice: e.target.value }))}
                  required
                />
              </div>
              <div className="form-group">
                <label>ID do Pedido</label>
                <input
                  className="form-control"
                  placeholder="Digite o ID do pedido"
                  value={form.orderId}
                  onChange={e => setForm(f => ({ ...f, orderId: e.target.value }))}
                  required
                />
              </div>
            </div>
            <div className="d-flex gap-10">
              <button 
                type="submit" 
                className="btn btn-primary"
                disabled={createMutation.isPending || updateMutation.isPending}
              >
                {createMutation.isPending || updateMutation.isPending 
                  ? (editing ? '⏳ Atualizando...' : '⏳ Criando...') 
                  : (editing ? '✅ Atualizar' : '➕ Criar Item')}
              </button>
              {editing && (
                <button 
                  type="button" 
                  className="btn btn-secondary"
                  onClick={handleCancelEdit}
                >
                  ❌ Cancelar
                </button>
              )}
            </div>
          </form>
        </div>

        {/* Lista de itens */}
        <div className="card">
          <h3>📋 Lista de Itens de Pedido</h3>
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Produto</th>
                  <th>Quantidade</th>
                  <th>Preço Unitário</th>
                  <th>Total</th>
                  <th>ID do Pedido</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {data && data.length > 0 ? (
                  data.map(i => (
                    <tr key={i.id}>
                      <td>
                        <strong>{i.productName}</strong>
                      </td>
                      <td>
                        <span className="badge badge-pending">
                          {i.quantity}
                        </span>
                      </td>
                      <td>
                        <span className="badge badge-processing">
                          R$ {parseFloat(i.unitPrice).toFixed(2)}
                        </span>
                      </td>
                      <td>
                        <span className="badge badge-completed">
                          R$ {(i.quantity * i.unitPrice).toFixed(2)}
                        </span>
                      </td>
                      <td>
                        <strong>#{i.orderId?.slice(0, 8) || 'N/A'}</strong>
                      </td>
                      <td>
                        <div className="d-flex gap-10">
                          <button 
                            className="btn btn-secondary"
                            onClick={() => handleEdit(i)}
                            disabled={deleteMutation.isPending}
                          >
                            ✏️ Editar
                          </button>
                          <button 
                            className="btn btn-danger"
                            onClick={() => handleDelete(i)}
                            disabled={deleteMutation.isPending}
                          >
                            {deleteMutation.isPending ? '⏳ Removendo...' : '🗑️ Remover'}
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="6" className="text-center">
                      📭 Nenhum item de pedido encontrado
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