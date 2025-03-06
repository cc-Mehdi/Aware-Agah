import { API_BASE_URL, getToken } from "./../../config.js";

export const getNotifications = async ({ userId, alarmEnglishName }) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Notification/GetNotifications/${userId}/${alarmEnglishName}`, {
            method: 'Get',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json',
                authorization: `Bearer ${getToken()}`
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