// Serviço de API para Customer
import axios from 'axios';

const API_URL = '/Customer';

export const getCustomers = async () => {
  console.log('Fazendo requisição GET para:', API_URL);
  try {
    const response = await axios.get(API_URL);
    console.log('Resposta GET customers:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição getCustomers:', error);
    throw error;
  }
};

export const getCustomerById = (id) => axios.get(`${API_URL}/${id}`);

export const createCustomer = async (data) => {
  console.log('Fazendo requisição POST para:', API_URL);
  console.log('Dados enviados:', data);
  try {
    const response = await axios.post(API_URL, data);
    console.log('Resposta POST customer:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição createCustomer:', error);
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

export const updateCustomer = async (id, data) => {
  console.log('Fazendo requisição PUT para:', `${API_URL}/${id}`, data);
  try {
    const response = await axios.put(`${API_URL}/${id}`, data);
    console.log('Resposta PUT customer:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição updateCustomer:', error);
    throw error;
  }
};

export const deleteCustomer = async (id) => {
  console.log('Fazendo requisição DELETE para:', `${API_URL}/${id}`);
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    console.log('Resposta DELETE customer:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição deleteCustomer:', error);
    throw error;
  }
}; 