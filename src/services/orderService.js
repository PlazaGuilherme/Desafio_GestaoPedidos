export async function fetchOrders() {
    const response = await fetch('http://localhost:5230/orders');
    if (!response.ok) {
        throw new Error('Erro ao buscar pedidos');
    }
    return response.json();
}       