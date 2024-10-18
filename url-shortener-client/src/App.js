import FingerprintJS from '@fingerprintjs/fingerprintjs';
import axios from 'axios';
import { useEffect, useState } from 'react';
import './App.css';
// import './components/Nav.jsx';
const apiUrl = process.env.REACT_APP_NET_API_URL
console.log(apiUrl)
// Initialize an agent at application startup.
const fpPromise = FingerprintJS.load()

function ShortenedUrlList(props) {
    const { urls, setShortUrls } = props;
    return (
        <div className="flex flex-col p-2">
            {urls.map((url, index) => (
                <div key={index} className="badge badge-info gap-2 mt-2">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        className="inline-block h-4 w-4 stroke-current"
                        onClick={() => deleteUrl(url)}>
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth="2"
                            d="M6 18L18 6M6 6l12 12"></path>
                    </svg>
                    {`http://localhost:3000/${url.shortUrl} - ${url.longUrl} - ${url.visitCount}`}
                </div>
            ))
            }
        </div>
    );

    async function deleteUrl(url) {
        try {
            const fp = await fpPromise;
            const result = await fp.get();
            const userFingerprint = result.visitorId;
            console.log(userFingerprint)
            console.log(url.shortUrl)
            let response = await axios.delete(`${apiUrl}/url/${url.shortUrl}`, {
                // userFingerprint,
                // shortenedUrl: url.shortUrl,
            });
            console.log(response)
            if (response.data === true) {
                const filteredUrls = urls.filter(u => u.shortUrl !== url.shortUrl);
                setShortUrls(filteredUrls)
            }
        } catch (err) {
            console.log(err)
        }
    }
}

function App() {
    const [urlText, setUrlText] = useState("");
    const [shortUrls, setShortUrls] = useState([]);
    const handleChange = (event) => {
        const value = event.target.value;
        setUrlText(value);
    };

    // const anonymousUserShortUrls = ["https://test.com","https://abc.com"];
    useEffect(() => {
        async function loadUserUrls() {
            const fp = await fpPromise;
            const result = await fp.get();
            const userFingerprint = result.visitorId;
            try {
                let response = await axios.get(`${apiUrl}/url?userFingerprint=${userFingerprint}`);
                console.log(response)
                setShortUrls(response.data)
            } catch (err) {
                console.log(err)
            }
        }
        loadUserUrls()
    }, []);

    return (
        <div className='container mx-auto px-4'>
            <input
                type="text"
                placeholder="Enter url"
                className="input input-bordered w-full max-w-xs"
                onChange={handleChange}
                value={urlText} />
            <button className="btn btn-primary" onClick={shortenUrl}>Shorten</button>

            <ShortenedUrlList urls={shortUrls} setShortUrls={setShortUrls} />
        </div>
    );

    async function shortenUrl() {
        const fp = await fpPromise;
        const result = await fp.get();
        const userFingerprint = result.visitorId;
        console.log(userFingerprint);
        try {
            let response = await axios.post(`${apiUrl}/url`, {
                url: urlText,
                userFingerprint
            });
            console.log(response)
            setShortUrls([...shortUrls, response.data])
            setUrlText("")
        } catch (err) {
            console.log(err)
        }
    }
}

export default App;