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

  if (isLoading) return <div>Carregando...</div>;
  if (error) return <div>Erro ao carregar itens do pedido: {error.message}</div>;

  return (
    <div>
      <h2>Itens do Pedido</h2>
      <p>Total de itens: {data ? data.length : 0}</p>
      
      {/* Formulário de criação/edição */}
      <div style={{ border: '1px solid #ccc', padding: 20, marginBottom: 20 }}>
        <h3>{editing ? 'Editar Item de Pedido' : 'Criar Novo Item de Pedido'}</h3>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 10 }}>
            <label>ID do Produto: </label>
            <input
              placeholder="ID do produto"
              value={form.productId}
              onChange={e => setForm(f => ({ ...f, productId: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>Nome do Produto: </label>
            <input
              placeholder="Nome do produto"
              value={form.productName}
              onChange={e => setForm(f => ({ ...f, productName: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>Quantidade: </label>
            <input
              placeholder="Quantidade"
              type="number"
              min="1"
              value={form.quantity}
              onChange={e => setForm(f => ({ ...f, quantity: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>Preço Unitário: </label>
            <input
              placeholder="Preço unitário"
              type="number"
              step="0.01"
              min="0"
              value={form.unitPrice}
              onChange={e => setForm(f => ({ ...f, unitPrice: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>ID do Pedido: </label>
            <input
              placeholder="ID do pedido"
              value={form.orderId}
              onChange={e => setForm(f => ({ ...f, orderId: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <button type="submit" disabled={createMutation.isPending || updateMutation.isPending}>
            {createMutation.isPending || updateMutation.isPending 
              ? (editing ? 'Atualizando...' : 'Criando...') 
              : (editing ? 'Atualizar' : 'Criar Item')}
          </button>
          {editing && (
            <button type="button" onClick={handleCancelEdit} style={{ marginLeft: 10 }}>
              Cancelar
            </button>
          )}
        </form>
      </div>

      {/* Lista de itens */}
      <h3>Lista de Itens de Pedido</h3>
      <table border="1" cellPadding="8" style={{ width: '100%' }}>
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
                <td>{i.productName}</td>
                <td>{i.quantity}</td>
                <td>R$ {i.unitPrice}</td>
                <td>R$ {(i.quantity * i.unitPrice).toFixed(2)}</td>
                <td>{i.orderId}</td>
                <td>
                  <button 
                    onClick={() => handleEdit(i)}
                    disabled={deleteMutation.isPending}
                    style={{ marginRight: 5 }}
                  >
                    Editar
                  </button>
                  <button 
                    onClick={() => handleDelete(i)}
                    disabled={deleteMutation.isPending}
                    style={{ backgroundColor: '#ff4444', color: 'white' }}
                  >
                    {deleteMutation.isPending ? 'Removendo...' : 'Remover'}
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6">Nenhum item de pedido encontrado</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
} 