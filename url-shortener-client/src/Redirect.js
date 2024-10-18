import axios from "axios";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const apiUrl = process.env.REACT_APP_NET_API_URL

export default function Redirect(props) {
    const [longUrl, setLongUrl] = useState('')
    const [shouldRedirect, setShoulRedirect] = useState(false)
    const { shortUrl } = useParams();

    useEffect(() => {
        console.log('ran once')
        getLongUrl(shortUrl);
    }, []);

    useEffect(() => {
        if (longUrl) {
            setTimeout(async () => {
                console.log(longUrl)
                await incrementVisitCount(shortUrl)
                setShoulRedirect(true)
            }, 3000)
        }
    }, [longUrl])

    if (shouldRedirect) {
        window.location.href = longUrl
        return;
    } else {
        return (
            <div>
                <h1>Redirect</h1>
                <p>You are being redirected to {longUrl} ...</p>
            </div>
        );
    }
    

    async function getLongUrl(shortUrl) {
        try {
            console.log(shortUrl)
            const response = await axios.get(`${apiUrl}/url/${shortUrl}`)
            console.log(response)
            console.log(response.data)
            setLongUrl(response.data)
        } catch (error) {
            console.log(error)
        }
    }

    async function incrementVisitCount(shortUrl) {
        try {
            const response = await axios.put(`${apiUrl}/url/${shortUrl}`)
            console.log(response)
        } catch (error) {
            console.log(error)
        }
    }
}