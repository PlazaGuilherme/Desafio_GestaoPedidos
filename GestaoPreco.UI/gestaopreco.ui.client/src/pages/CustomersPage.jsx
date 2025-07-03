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

  if (isLoading) return <div className="loading">⏳ Carregando clientes...</div>;
  if (error) return <div className="error">❌ Erro ao carregar clientes: {error.message}</div>;

  return (
    <div className="fade-in">
      <div className="card">
        <div className="d-flex justify-between align-center mb-20">
          <h2>👥 Gerenciamento de Clientes</h2>
          <div className="badge badge-pending">
            Total: {data ? data.length : 0} clientes
          </div>
        </div>
        
        {/* Formulário de criação/edição */}
        <div className="card">
          <h3>{editing ? '✏️ Editar Cliente' : '➕ Criar Novo Cliente'}</h3>
          <form onSubmit={handleSubmit}>
            <div className="grid grid-2">
              <div className="form-group">
                <label>Nome do Cliente</label>
                <input
                  className="form-control"
                  placeholder="Digite o nome completo"
                  value={form.name}
                  onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  className="form-control"
                  placeholder="Digite o email"
                  type="email"
                  value={form.email}
                  onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
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
                  : (editing ? '✅ Atualizar' : '➕ Criar Cliente')}
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

        {/* Lista de clientes */}
        <div className="card">
          <h3>📋 Lista de Clientes</h3>
          <div className="table-container">
            <table className="table">
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
                      <td>
                        <strong>{c.name}</strong>
                      </td>
                      <td>{c.email}</td>
                      <td>
                        <div className="d-flex gap-10">
                          <button 
                            className="btn btn-secondary"
                            onClick={() => handleEdit(c)}
                            disabled={deleteMutation.isPending}
                          >
                            ✏️ Editar
                          </button>
                          <button 
                            className="btn btn-danger"
                            onClick={() => handleDelete(c)}
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
                    <td colSpan="3" className="text-center">
                      📭 Nenhum cliente encontrado
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