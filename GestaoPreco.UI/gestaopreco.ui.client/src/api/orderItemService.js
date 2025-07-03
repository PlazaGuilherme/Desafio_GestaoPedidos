// Serviço de API para OrderItem
import axios from 'axios';

const API_URL = '/OrderItens';

export const getOrderItems = () => axios.get(API_URL);
export const getOrderItemById = (id) => axios.get(`${API_URL}/${id}`);
export const createOrderItem = (data) => axios.post(API_URL, data);
export const updateOrderItem = (id, data) => axios.put(`${API_URL}/${id}`, data);
export const deleteOrderItem = (id) => axios.delete(`${API_URL}/${id}`); 