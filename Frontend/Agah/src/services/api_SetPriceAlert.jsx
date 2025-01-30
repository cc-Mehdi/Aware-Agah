export const sendPriceAlert = async ({ userId, product, minPrice, maxPrice, createdAt }) => {
    try {
        const response = await fetch('https://localhost:44314/api/PriceAlert', {
            method: 'POST',
            headers: {
                'Accept': '*/*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userId,
                product,
                minPrice,
                maxPrice,
                createdAt: createdAt || new Date().toISOString()
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