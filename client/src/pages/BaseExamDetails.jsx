import React, { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const BaseExamDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [exam, setExam] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchExam = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.GET_BY_ID(id));
                setExam(response.data);
            } catch (error) {
                console.error('Failed to fetch exam details:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchExam();
    }, [id]);

    const handleGenerate = async () => {
        try {
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(id));
            alert('Új ZH példány sikeresen generálva!');
            navigate('/generalt-zhk');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Nem sikerült a ZH generálása.');
        }
    };

    if (loading) return <div className="page">Betöltés...</div>;
    if (!exam) return <div className="page">A ZH nem található.</div>;

    return (
        <div className="page base-exam-details-page">
            <div className="header-actions">
                <h2>Alap ZH Részletek #{exam.id}</h2>
                <Link to="/alap-zhk" className="btn btn-secondary">Vissza a listához</Link>
            </div>

            <div className="card">
                <div className="form-group">
                    <label>ZH Neve</label>
                    <div className="static-value">{exam.nev}</div>
                </div>
                <div className="form-group">
                    <label>Minta ZH Azonosító</label>
                    <div className="static-value">{exam.mintaZh}</div>
                </div>
                <div className="form-group">
                    <label>Tematika</label>
                    <div className="static-value">{exam.tematika}</div>
                </div>
                <div className="form-group">
                    <label>Leírás</label>
                    <div className="static-value">{exam.temakorLeiras}</div>
                </div>
                <div className="form-row">
                    <div className="form-group">
                        <label>Feladatok Száma</label>
                        <div className="static-value">{exam.feladatokSzama}</div>
                    </div>
                    <div className="form-group">
                        <label>Nyelv</label>
                        <div className="static-value">{exam.programozasiNyelv}</div>
                    </div>
                    <div className="form-group">
                        <label>Nehézség</label>
                        <div className="static-value">{exam.nehezseg}</div>
                    </div>
                </div>

                <div className="mt-4">
                    <button onClick={handleGenerate} className="btn btn-primary">
                        Új Példány Generálása
                    </button>
                </div>
            </div>
        </div>
    );
};

export default BaseExamDetails;
