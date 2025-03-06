import { API_BASE_URL } from "./../../config.js";

export const api_HandleRegister = async (email, password) => {
    try {
        const response = await fetch(`${API_BASE_URL}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();
        return data; // Return the response data
    } catch (error) {
        console.error('Registration failed', error);
        throw error; // Re-throw the error if needed
    }
};
