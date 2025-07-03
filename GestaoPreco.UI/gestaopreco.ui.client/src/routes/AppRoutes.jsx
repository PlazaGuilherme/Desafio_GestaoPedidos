import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import CustomersPage from '../pages/CustomersPage';
import ProductsPage from '../pages/ProductsPage';
import OrdersPage from '../pages/OrdersPage';
import OrderItemsPage from '../pages/OrderItemsPage';

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <div className="header fade-in">
        <h1>🏪 Gestão de Preços</h1>
        <nav className="nav">
          <Link to="/customers" className="nav-link">👥 Clientes</Link>
          <Link to="/products" className="nav-link">📦 Produtos</Link>
          <Link to="/orders" className="nav-link">📋 Pedidos</Link>
          <Link to="/order-items" className="nav-link">🛒 Itens</Link>
        </nav>
      </div>
      
      <Routes>
        <Route path="/customers" element={<CustomersPage />} />
        <Route path="/products" element={<ProductsPage />} />
        <Route path="/orders" element={<OrdersPage />} />
        <Route path="/order-items" element={<OrderItemsPage />} />
        <Route path="*" element={
          <div className="card text-center">
            <h2>Bem-vindo ao Sistema de Gestão de Preços! 🎉</h2>
            <p className="mt-20">Selecione uma opção no menu acima para começar.</p>
          </div>
        } />
      </Routes>
    </BrowserRouter>
  );
} 