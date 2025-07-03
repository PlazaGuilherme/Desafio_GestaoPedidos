import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [preco, setPreco] = useState(null);
    const [loading, setLoading] = useState(true);
    const [erro, setErro] = useState(null);

    useEffect(() => {
        // Substitua 1 pelo ID do produto desejado
        fetch('http://localhost:5230/Price/1')
            .then(response => {
                if (!response.ok) throw new Error('Erro ao buscar pre�o');
                return response.json();
            })
            .then(data => {
                setPreco(data);
                setLoading(false);
            })
            .catch(error => {
                setErro(error.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <div>Carregando...</div>;
    if (erro) return <div>Erro: {erro}</div>;

    return (
        <div>
            <h1>Pre�o do Produto</h1>
            <p>ID: {preco.productId}</p>
            <p>Pre�o: {preco.price} {preco.coin}</p>
        </div>
    );
}

export default App;