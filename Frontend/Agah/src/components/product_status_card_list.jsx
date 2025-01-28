import React, { useEffect, useState } from 'react';
import ProductStatusCard from './product_status_card'; // Adjust the import path if necessary

const ProductStatusCardList = () => {
    // State to store the products
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Fetch product data when the component mounts
    useEffect(() => {
        // Function to fetch data from API
        const fetchProductData = async () => {
            try {
                const response = await fetch('https://api.metalpriceapi.com/v1/latest?api_key=05c5d4d6f8fba5579c62aafd2efb52f8&base=USD&currencies=IRR'); // Replace with your API endpoint
                if (!response.ok) {
                    throw new Error('Failed to fetch products');
                }
                const data = await response.json();
                setProducts(data);  // Assuming the response is an array of product objects
                setLoading(false);   // Data has been loaded

                console.log("قیمت هر 1 دلار به تومان برابر است با : " + String(data.rates["IRR"]).split('.')[0]);

            } catch (err) {
                setError(err.message);
                setLoading(false);  // Even on error, stop the loading state
            }
        };

        fetchProductData(); // Trigger the API call
    }, []); // Empty dependency array means this runs only once when the component mounts

    if (loading) {
        return <div>Loading...</div>; // Display loading message
    }

    if (error) {
        return <div>Error: {error}</div>; // Display error message if fetching fails
    }

    return (
        <div className="product-status-card-list">
            {products.map((product) => (
                <ProductStatusCard
                    key={product.id}  // Unique key for each item
                    name={product.name}
                    amount={product.amount}
                    changedPercent={product.changedPercent}
                    changedAmount={product.changedAmount}
                />
            ))}
        </div>
    );
};

export default ProductStatusCardList;
