import { API_BASE_URL, getToken } from "./../../config.js";

export const putUser = async (userEmail, userNewFullname) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Users/User`, {
            method: 'PUT', // Use uppercase 'PUT' for HTTP methods
            headers: {
                'Authorization': `Bearer ${getToken()}`,
                'Content-Type': 'application/json', // Set the content type to JSON
            },
            body: JSON.stringify({
                "Email": userEmail,
                "newFullname": userNewFullname
            }),
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