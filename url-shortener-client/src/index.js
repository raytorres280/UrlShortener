import { createRoot } from 'react-dom/client';
import { createBrowserRouter, Link, RouterProvider, } from "react-router-dom";
import App from './App.js';
// import Redirecter from "./components/Redirecter.js";
import './index.css';
import Redirect from './Redirect.js';

const router = createBrowserRouter([
  // route that redirects to long url
  {
    path: "/app",
    element: (<App />),
  },
  {
    path: "/:shortUrl",
    element: (<Redirect />),
    errorElement: (<div>
      <Link to={"/app"}>Go to app to create links</Link>
    </div>
    )
  },
  {
    path: "/",
    element: (<div className='underline text-blue-600 hover:text-blue-800 visited:text-purple-600'>
      <Link to={"/app"}>Click here to go to app to create links</Link>
    </div>
    )
  }
]);

createRoot(document.getElementById('root')).render(
  <RouterProvider router={router} />,
)