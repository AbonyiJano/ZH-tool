import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const BaseExamsList = () => {
    const [exams, setExams] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchExams = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.EXAM.LIST);
                setExams(response.data);
            } catch (error) {
                console.error('Failed to fetch exams:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchExams();
    }, []);

    const handleGenerate = async (examId) => {
        try {
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(examId));
            alert('Új ZH példány sikeresen generálva!');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Nem sikerült a ZH generálása.');
        }
    };

    if (loading) return <div className="page">Betöltés...</div>;

    return (
        <div className="page base-exams-page">
            <h2>Alap ZH-k (Sablonok)</h2>
            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Azonosító</th>
                            <th>Név</th>
                            <th>Műveletek</th>
                        </tr>
                    </thead>
                    <tbody>
                        {exams.map((exam) => (
                            <tr key={exam.id}>
                                <td>{exam.id}</td>
                                <td>{exam.nev}</td>
                                <td>
                                    <div style={{ display: 'flex', gap: '0.5rem' }}>
                                        <Link to={`/alap-zhk/${exam.id}`} className="btn btn-secondary btn-sm">
                                            Részletek
                                        </Link>
                                        <button
                                            className="btn btn-primary btn-sm"
                                            onClick={() => handleGenerate(exam.id)}
                                        >
                                            Példány Generálása
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {exams.length === 0 && <p className="text-center mt-4">Nincsenek alap ZH-k.</p>}
            </div>
        </div>
    );
};

export default BaseExamsList;
