import { API_BASE_URL } from "./../../config.js";

export const getProductsLog = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/Product/GetProductsLog`, {
            method: 'Get',
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        return data.result;
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};