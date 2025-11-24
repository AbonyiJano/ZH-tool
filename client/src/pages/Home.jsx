import React from 'react';
import { Link } from 'react-router-dom';

const Home = () => {
    return (
        <div className="page home-page">
            <h1>Tanári Vezérlőpult</h1>
            <p>Üdvözöljük a ZH-Tool Tanári Felületén.</p>
            <div className="dashboard-grid">
                <Link to="/uj-zh" className="card dashboard-card">
                    <h3>Új ZH Létrehozása</h3>
                    <p>Új ZH-k és feladatok tervezése.</p>
                </Link>
                <Link to="/hallgatok" className="card dashboard-card">
                    <h3>Hallgatók Kezelése</h3>
                    <p>Hallgatók regisztrálása és megtekintése.</p>
                </Link>
                <Link to="/javitas" className="card dashboard-card">
                    <h3>Javítás</h3>
                    <p>Hallgatói megoldások beküldése és javítása.</p>
                </Link>
            </div>
        </div>
    );
};

export default Home;
