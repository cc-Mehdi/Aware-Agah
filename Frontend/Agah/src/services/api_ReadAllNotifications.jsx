import { API_BASE_URL } from "./../../config.js";

export const readAllNotifications = async ({ userId }) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Notification/ReadAllNotifications/${userId}`, {
            method: 'Get',
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