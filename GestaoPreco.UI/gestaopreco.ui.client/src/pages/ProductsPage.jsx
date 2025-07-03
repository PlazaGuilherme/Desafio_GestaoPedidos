import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getProducts, createProduct, deleteProduct } from '../api/productService';

export default function ProductsPage() {
  const queryClient = useQueryClient();
  const { data, isLoading, error } = useQuery({
    queryKey: ['products'],
    queryFn: getProducts,
    select: res => res.data
  });
  const [form, setForm] = useState({ name: '', price: '' });

  const createMutation = useMutation({
    mutationFn: createProduct,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] });
      setForm({ name: '', price: '' });
      alert('Produto criado com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao criar produto: ' + error.message);
    }
  });
  
  const deleteMutation = useMutation({
    mutationFn: deleteProduct,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['products'] });
      alert('✅ Produto removido com sucesso!');
    },
    onError: (error) => {
      console.error('Erro detalhado do delete:', error);
      const errorMessage = error.response?.data || error.message || 'Erro desconhecido';
      alert(`❌ Erro ao remover produto: ${errorMessage}`);
    }
  });

  const handleSubmit = e => {
    e.preventDefault();
    createMutation.mutate(form);
  };

  const handleDelete = (product) => {
    if (window.confirm(`Tem certeza que deseja remover o produto "${product.name}"?`)) {
      deleteMutation.mutate(product.id);
    }
  };

  if (isLoading) return <div className="loading">⏳ Carregando produtos...</div>;
  if (error) return <div className="error">❌ Erro ao carregar produtos: {error.message}</div>;

  return (
    <div className="fade-in">
      <div className="card">
        <div className="d-flex justify-between align-center mb-20">
          <h2>📦 Gerenciamento de Produtos</h2>
          <div className="badge badge-processing">
            Total: {data ? data.length : 0} produtos
          </div>
        </div>
        
        {/* Formulário de criação */}
        <div className="card">
          <h3>➕ Criar Novo Produto</h3>
          <form onSubmit={handleSubmit}>
            <div className="grid grid-2">
              <div className="form-group">
                <label>Nome do Produto</label>
                <input
                  className="form-control"
                  placeholder="Digite o nome do produto"
                  value={form.name}
                  onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
                  required
                />
              </div>
              <div className="form-group">
                <label>Preço (R$)</label>
                <input
                  className="form-control"
                  placeholder="0.00"
                  type="number"
                  step="0.01"
                  min="0"
                  value={form.price}
                  onChange={e => setForm(f => ({ ...f, price: e.target.value }))}
                  required
                />
              </div>
            </div>
            <button 
              type="submit" 
              className="btn btn-primary"
              disabled={createMutation.isPending}
            >
              {createMutation.isPending ? '⏳ Criando...' : '➕ Criar Produto'}
            </button>
          </form>
        </div>

        {/* Lista de produtos */}
        <div className="card">
          <h3>📋 Lista de Produtos</h3>
          <div className="table-container">
            <table className="table">
              <thead>
                <tr>
                  <th>Produto</th>
                  <th>Preço</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {data && data.length > 0 ? (
                  data.map(p => (
                    <tr key={p.id}>
                      <td>
                        <strong>{p.name}</strong>
                      </td>
                      <td>
                        <span className="badge badge-completed">
                          R$ {parseFloat(p.price).toFixed(2)}
                        </span>
                      </td>
                      <td>
                        <button 
                          className="btn btn-danger"
                          onClick={() => handleDelete(p)}
                          disabled={deleteMutation.isPending}
                        >
                          {deleteMutation.isPending ? '⏳ Removendo...' : '🗑️ Remover'}
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="3" className="text-center">
                      📭 Nenhum produto encontrado
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