import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const GeneratedExamsList = () => {
    const [generatedExams, setGeneratedExams] = useState([]);
    const [baseExams, setBaseExams] = useState({});
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [genResponse, baseResponse] = await Promise.all([
                    apiClient.get(ENDPOINTS.EXAM.LIST_GENERATED),
                    apiClient.get(ENDPOINTS.EXAM.LIST)
                ]);

                setGeneratedExams(genResponse.data);

                // Create a map of base exams for easy lookup
                const baseMap = {};
                baseResponse.data.forEach(exam => {
                    baseMap[exam.id] = exam;
                });
                setBaseExams(baseMap);
            } catch (error) {
                console.error('Failed to fetch data:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const handleGenerate = async (baseExamId) => {
        try {
            if (!baseExamId) return;
            await apiClient.post(ENDPOINTS.EXAM.GENERATE(baseExamId));
            // Refresh list
            const response = await apiClient.get(ENDPOINTS.EXAM.LIST_GENERATED);
            setGeneratedExams(response.data);
            alert('Új ZH példány sikeresen generálva!');
        } catch (error) {
            console.error('Failed to generate exam:', error);
            alert('Nem sikerült a ZH generálása.');
        }
    };

    if (loading) return <div className="page">Betöltés...</div>;

    return (
        <div className="page generated-exams-page">
            <div className="header-actions">
                <h2>Generált ZH-k</h2>
                <div className="generate-action">
                    <select id="baseExamSelect" className="select-input">
                        <option value="">Válasszon Alap ZH-t a generáláshoz</option>
                        {Object.values(baseExams).map(exam => (
                            <option key={exam.id} value={exam.id}>{exam.nev}</option>
                        ))}
                    </select>
                    <button
                        className="btn btn-primary"
                        onClick={() => handleGenerate(document.getElementById('baseExamSelect').value)}
                    >
                        Új Példány Generálása
                    </button>
                </div>
            </div>

            <div className="table-container">
                <table className="data-table">
                    <thead>
                        <tr>
                            <th>Azonosító</th>
                            <th>Alap ZH Neve</th>
                            <th>Generálás Ideje</th>
                            <th>Műveletek</th>
                        </tr>
                    </thead>
                    <tbody>
                        {generatedExams.map((exam) => (
                            <tr key={exam.id}>
                                <td>{exam.id}</td>
                                <td>{baseExams[exam.parentZhId]?.nev || 'Ismeretlen'}</td>
                                <td>{new Date(exam.generationTime).toLocaleString()}</td>
                                <td>
                                    <Link to={`/generalt-zhk/${exam.id}/feladatok`} className="btn btn-secondary btn-sm">
                                        Feladatok Megtekintése
                                    </Link>
                                    <Link
                                        to="/javitas"
                                        state={{ generaltZhId: exam.id }}
                                        className="btn btn-primary btn-sm"
                                        style={{ marginLeft: '0.5rem' }}
                                    >
                                        Megoldás Beküldése
                                    </Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                {generatedExams.length === 0 && <p className="text-center mt-4">Nincsenek generált ZH-k.</p>}
            </div>
        </div>
    );
};

export default GeneratedExamsList;
