import { API_BASE_URL, getToken } from "./../../config.js";

export const setPriceRangeReservation = async ({ userId, productId, alarmId, minPrice, maxPrice }) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Reserve/SetPriceRangeReservation`, {
            method: 'POST',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json',
                authorization: `Bearer ${getToken()}`
            },
            body: JSON.stringify({
                userId,
                productId,
                alarmId,
                minPrice,
                maxPrice,
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        return { status: response.status, ...data };
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};