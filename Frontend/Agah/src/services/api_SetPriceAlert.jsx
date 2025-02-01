export const sendPriceAlert = async ({ userId, productId, alarmId, minPrice, maxPrice }) => {
    try {
        const response = await fetch('https://localhost:44314/api/PriceAlert', {
            method: 'POST',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json'
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
        return data;
    } catch (error) {
        console.error('Error:', error);
        throw error;
    }
};