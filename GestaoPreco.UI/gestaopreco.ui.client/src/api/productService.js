// Serviço de API para Product
import axios from 'axios';

const API_URL = '/Product';

export const getProducts = async () => {
  console.log('Fazendo requisição GET para:', API_URL);
  try {
    const response = await axios.get(API_URL);
    console.log('Resposta GET products:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição getProducts:', error);
    throw error;
  }
};

export const getProductById = (id) => axios.get(`${API_URL}/${id}`);
export const createProduct = (data) => axios.post(API_URL, data);

export const deleteProduct = async (id) => {
  console.log('Fazendo requisição DELETE para:', `${API_URL}/${id}`);
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    console.log('Resposta DELETE product:', response);
    return response;
  } catch (error) {
    console.error('Erro na requisição deleteProduct:', error);
    throw error;
  }
}; 