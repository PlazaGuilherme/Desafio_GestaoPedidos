// Serviço de API para Order
import axios from 'axios';

const API_URL = '/api/Order';

export const getOrders = async () => {
  console.log('Fazendo requisição para:', API_URL);
  try {
    const response = await axios.get(API_URL);
    console.log('Resposta recebida:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição getOrders:', error);
    throw error;
  }
};

export const getOrderById = (id) => axios.get(`${API_URL}/${id}`);
export const getOrdersWithItems = () => axios.get(`${API_URL}/with-items`);
export const getOrderWithItemsById = (id) => axios.get(`${API_URL}/${id}/with-items`);
export const createOrder = (data) => axios.post(API_URL, data);
export const createOrderWithItems = async (data) => {
  console.log('Fazendo requisição POST para:', `${API_URL}/create-with-items`);
  console.log('Dados enviados:', JSON.stringify(data, null, 2));
  try {
    const response = await axios.post(`${API_URL}/create-with-items`, data);
    console.log('Resposta POST createOrderWithItems:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição createOrderWithItems:', error);
    console.error('Detalhes do erro:', {
      message: error.message,
      status: error.response?.status,
      statusText: error.response?.statusText,
      data: error.response?.data,
      headers: error.response?.headers
    });
    throw error;
  }
};
export const updateOrder = (id, data) => axios.put(`${API_URL}/${id}`, data);

export const deleteOrder = async (id) => {
  console.log('Fazendo requisição DELETE para:', `${API_URL}/${id}`);
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    console.log('Resposta DELETE order:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição deleteOrder:', error);
    throw error;
  }
}; 