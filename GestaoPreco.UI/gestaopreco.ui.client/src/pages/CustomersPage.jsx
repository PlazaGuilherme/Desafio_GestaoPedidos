import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getCustomers, createCustomer, updateCustomer, deleteCustomer } from '../api/customerService';

export default function CustomersPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({
    queryKey: ['customers'],
    queryFn: getCustomers,
    select: res => res.data
  });
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState({ name: '', email: '' });

  const createMutation = useMutation({
    mutationFn: createCustomer,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] });
      setForm({ name: '', email: '' });
      alert('Cliente criado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao criar cliente: ' + error.message);
    }
  });
  
  const updateMutation = useMutation({
    mutationFn: ({ id, data }) => updateCustomer(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] });
      setEditing(null);
      setForm({ name: '', email: '' });
      alert('Cliente atualizado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao atualizar cliente: ' + error.message);
    }
  });
  
  const deleteMutation = useMutation({
    mutationFn: deleteCustomer,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customers'] });
      alert('Cliente removido com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao remover cliente: ' + error.message);
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

  const handleEdit = customer => {
    setEditing(customer);
    setForm({ name: customer.name, email: customer.email });
  };

  const handleDelete = (customer) => {
    if (window.confirm(`Tem certeza que deseja remover o cliente "${customer.name}"?`)) {
      deleteMutation.mutate(customer.id);
    }
  };

  const handleCancelEdit = () => {
    setEditing(null);
    setForm({ name: '', email: '' });
  };

  if (isLoading) return <div>Carregando...</div>;
  if (error) return <div>Erro ao carregar clientes: {error.message}</div>;

  return (
    <div>
      <h2>Clientes</h2>
      <p>Total de clientes: {data ? data.length : 0}</p>
      
      {/* Formulário de criação/edição */}
      <div style={{ border: '1px solid #ccc', padding: 20, marginBottom: 20 }}>
        <h3>{editing ? 'Editar Cliente' : 'Criar Novo Cliente'}</h3>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 10 }}>
            <label>Nome: </label>
            <input
              placeholder="Nome do cliente"
              value={form.name}
              onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>Email: </label>
            <input
              placeholder="Email do cliente"
              type="email"
              value={form.email}
              onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <button type="submit" disabled={createMutation.isPending || updateMutation.isPending}>
            {createMutation.isPending || updateMutation.isPending 
              ? (editing ? 'Atualizando...' : 'Criando...') 
              : (editing ? 'Atualizar' : 'Criar Cliente')}
          </button>
          {editing && (
            <button type="button" onClick={handleCancelEdit} style={{ marginLeft: 10 }}>
              Cancelar
            </button>
          )}
        </form>
      </div>

      {/* Lista de clientes */}
      <h3>Lista de Clientes</h3>
      <table border="1" cellPadding="8" style={{ width: '100%' }}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Email</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {data && data.length > 0 ? (
            data.map(c => (
              <tr key={c.id}>
                <td>{c.name}</td>
                <td>{c.email}</td>
                <td>
                  <button 
                    onClick={() => handleEdit(c)}
                    disabled={deleteMutation.isPending}
                    style={{ marginRight: 5 }}
                  >
                    Editar
                  </button>
                  <button 
                    onClick={() => handleDelete(c)}
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
              <td colSpan="3">Nenhum cliente encontrado</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
} 