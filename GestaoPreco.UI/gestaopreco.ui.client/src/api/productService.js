// Serviço de API para Product
import axios from 'axios';

const API_URL = '/Product';

export const getProducts = () => axios.get(API_URL);
export const getProductById = (id) => axios.get(`${API_URL}/${id}`);
export const createProduct = (data) => axios.post(API_URL, data);
export const deleteProduct = (id) => axios.delete(`${API_URL}/${id}`); 