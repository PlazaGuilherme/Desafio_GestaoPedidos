import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import CustomersPage from '../pages/CustomersPage';
import ProductsPage from '../pages/ProductsPage';
import OrdersPage from '../pages/OrdersPage';
import OrderItemsPage from '../pages/OrderItemsPage';

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <nav style={{ marginBottom: 20 }}>
        <Link to="/customers" style={{ marginRight: 10 }}>Clientes</Link>
        <Link to="/products" style={{ marginRight: 10 }}>Produtos</Link>
        <Link to="/orders" style={{ marginRight: 10 }}>Pedidos</Link>
        <Link to="/order-items">Itens do Pedido</Link>
      </nav>
      <Routes>
        <Route path="/customers" element={<CustomersPage />} />
        <Route path="/products" element={<ProductsPage />} />
        <Route path="/orders" element={<OrdersPage />} />
        <Route path="/order-items" element={<OrderItemsPage />} />
        <Route path="*" element={<div>Bem-vindo ao sistema de Gestão de Preço!</div>} />
      </Routes>
    </BrowserRouter>
  );
} 