import Home from './Pages/Home'
import ErrorHandler from "./components/ErrorHandler";
import { useEffect } from 'react';


function App() {
useEffect(() => {
        const link = document.createElement('link');
        link.href = 'https://fonts.googleapis.com/css2?family=Vazirmatn:wght@400;700&display=swap';
        link.rel = 'stylesheet';
        document.head.appendChild(link);
        document.body.style.fontFamily = 'Vazirmatn, sans-serif';
    }, []);
    return (
        <>
            <ErrorHandler />
            <Home />
        </>
    )
}

export default App