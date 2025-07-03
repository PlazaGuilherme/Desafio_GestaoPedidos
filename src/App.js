import React, { useEffect, useState } from 'react';
import { fetchOrders } from './services/orderService';

function App() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetchOrders()
            .then(data => setOrders(data))
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <div>Carregando pedidos...</div>;
    if (error) return <div>Erro: {error}</div>;

    return (
        <div>
            <h1>Pedidos</h1>
            <ul>
                {orders.map(order => (
                    <li key={order.id}>
                        Cliente: {order.customerId} | Data: {new Date(order.date).toLocaleDateString()} | Total: R$ {order.totalAmount}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;