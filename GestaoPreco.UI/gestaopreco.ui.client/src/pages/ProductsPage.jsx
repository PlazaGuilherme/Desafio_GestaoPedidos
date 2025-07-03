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
      alert('Produto removido com sucesso!');
    },
    onError: (error) => {
      alert('Erro ao remover produto: ' + error.message);
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

  if (isLoading) return <div>Carregando...</div>;
  if (error) return <div>Erro ao carregar produtos: {error.message}</div>;

  return (
    <div>
      <h2>Produtos</h2>
      <p>Total de produtos: {data ? data.length : 0}</p>
      
      {/* Formulário de criação */}
      <div style={{ border: '1px solid #ccc', padding: 20, marginBottom: 20 }}>
        <h3>Criar Novo Produto</h3>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 10 }}>
            <label>Nome: </label>
            <input
              placeholder="Nome do produto"
              value={form.name}
              onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <div style={{ marginBottom: 10 }}>
            <label>Preço: </label>
            <input
              placeholder="Preço do produto"
              type="number"
              step="0.01"
              min="0"
              value={form.price}
              onChange={e => setForm(f => ({ ...f, price: e.target.value }))}
              required
              style={{ width: '200px' }}
            />
          </div>
          <button type="submit" disabled={createMutation.isPending}>
            {createMutation.isPending ? 'Criando...' : 'Criar Produto'}
          </button>
        </form>
      </div>

      {/* Lista de produtos */}
      <h3>Lista de Produtos</h3>
      <table border="1" cellPadding="8" style={{ width: '100%' }}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Preço</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {data && data.length > 0 ? (
            data.map(p => (
              <tr key={p.id}>
                <td>{p.name}</td>
                <td>R$ {p.price}</td>
                <td>
                  <button 
                    onClick={() => handleDelete(p)}
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
              <td colSpan="3">Nenhum produto encontrado</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
} 