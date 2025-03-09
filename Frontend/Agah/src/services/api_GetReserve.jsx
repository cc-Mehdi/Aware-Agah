import { API_BASE_URL, getToken } from "./../../config.js";

export const getReserve = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/Reserve/Reserve`, {
            method: 'Get', // Use uppercase 'PUT' for HTTP methods
            headers: {
                'Authorization': `Bearer ${getToken()}`,
                'Content-Type': 'application/json', // Set the content type to JSON
            },
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};