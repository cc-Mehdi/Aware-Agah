import { API_BASE_URL } from "./../../config.js";

export const getNotifications = async ({ userId, alarmEnglishName }) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Notification/GetNotifications/${userId}/${alarmEnglishName}`, {
            method: 'Get',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json'
            }
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