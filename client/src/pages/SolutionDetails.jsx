import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import apiClient from '../api/client';
import { ENDPOINTS } from '../api/endpoints';

const SolutionDetails = () => {
    const { id } = useParams();
    const [solution, setSolution] = useState(null);
    const [loading, setLoading] = useState(true);
    const [editingEvaluation, setEditingEvaluation] = useState(false);
    const [editFormData, setEditFormData] = useState({
        pontszam: '',
        osszPontszam: '',
        llmVisszajelzes: ''
    });

    useEffect(() => {
        const fetchSolution = async () => {
            try {
                const response = await apiClient.get(ENDPOINTS.SOLUTION.GET_BY_ID(id));
                setSolution(response.data);
            } catch (error) {
                console.error('Failed to fetch solution:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchSolution();
    }, [id]);

    const handleEditClick = () => {
        if (solution.ertekeles) {
            setEditFormData({
                pontszam: solution.ertekeles.pontszam,
                osszPontszam: solution.ertekeles.osszPontszam,
                llmVisszajelzes: solution.ertekeles.llmVisszajelzes
            });
            setEditingEvaluation(true);
        }
    };

    const handleCancelEdit = () => {
        setEditingEvaluation(false);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditFormData(prev => ({
            ...prev,
            [name]: name === 'pontszam' || name === 'osszPontszam' ? parseInt(value) || 0 : value
        }));
    };

    const handleSaveEdit = async () => {
        try {
            await apiClient.put(ENDPOINTS.EVALUATION.UPDATE(solution.ertekeles.id), {
                id: solution.ertekeles.id,
                megoldasId: solution.id,
                pontszam: editFormData.pontszam,
                osszPontszam: editFormData.osszPontszam,
                llmVisszajelzes: editFormData.llmVisszajelzes,
                ertekelesDatuma: solution.ertekeles.ertekelesDatuma,
                hallgatoNeptunkod: solution.hallgatoNeptunkod
            });

            // Refresh solution data
            const response = await apiClient.get(ENDPOINTS.SOLUTION.GET_BY_ID(id));
            setSolution(response.data);
            setEditingEvaluation(false);
            alert('Értékelés sikeresen frissítve!');
        } catch (error) {
            console.error('Failed to update evaluation:', error);
            alert('Nem sikerült az értékelés frissítése.');
        }
    };

    if (loading) return <div className="page">Betöltés...</div>;
    if (!solution) return <div className="page">A megoldás nem található.</div>;

    return (
        <div className="page solution-details-page">
            <div className="header-actions">
                <h2>Megoldás Részletei #{solution.id}</h2>
                <div style={{ display: 'flex', gap: '1rem' }}>
                    <Link to={`/generalt-zhk/${solution.generaltZhId}/feladatok`} className="btn btn-primary">
                        Feladatok Megtekintése
                    </Link>
                    <Link to="/javitasok" className="btn btn-secondary">Vissza a Megoldásokhoz</Link>
                </div>
            </div>

            <div className="card">
                <div className="form-group">
                    <label>Hallgató Neptun Kódja</label>
                    <div className="static-value">{solution.hallgatoNeptunkod}</div>
                </div>
                <div className="form-group">
                    <label>Generált ZH Azonosító</label>
                    <div className="static-value">{solution.generaltZhId}</div>
                </div>
                <div className="form-group">
                    <label>Beküldött Megoldás</label>
                    <pre className="code-block">{solution.bekuldottMegoldas}</pre>
                </div>
                <div className="form-group">
                    <label>Beküldés Dátuma</label>
                    <div className="static-value">{new Date(solution.bekuldesDatuma).toLocaleString()}</div>
                </div>
            </div>

            {solution.ertekeles && (
                <div className="card" style={{ marginTop: '2rem' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
                        <h3>Értékelés</h3>
                        {!editingEvaluation && (
                            <button className="btn btn-secondary btn-sm" onClick={handleEditClick}>
                                Szerkesztés
                            </button>
                        )}
                    </div>

                    {editingEvaluation ? (
                        <>
                            <div className="form-group">
                                <label>Elért Pontszám</label>
                                <input
                                    type="number"
                                    name="pontszam"
                                    value={editFormData.pontszam}
                                    onChange={handleInputChange}
                                    className="form-control"
                                />
                            </div>
                            <div className="form-group">
                                <label>Összpontszám</label>
                                <input
                                    type="number"
                                    name="osszPontszam"
                                    value={editFormData.osszPontszam}
                                    onChange={handleInputChange}
                                    className="form-control"
                                />
                            </div>
                            <div className="form-group">
                                <label>Részletes Visszajelzés</label>
                                <textarea
                                    name="llmVisszajelzes"
                                    value={editFormData.llmVisszajelzes}
                                    onChange={handleInputChange}
                                    rows="10"
                                    className="form-control"
                                />
                            </div>
                            <div style={{ display: 'flex', gap: '1rem', marginTop: '1rem' }}>
                                <button className="btn btn-primary" onClick={handleSaveEdit}>
                                    Mentés
                                </button>
                                <button className="btn btn-secondary" onClick={handleCancelEdit}>
                                    Mégse
                                </button>
                            </div>
                        </>
                    ) : (
                        <>
                            <div className="form-group">
                                <label>Pontszám</label>
                                <div className="static-value">
                                    {solution.ertekeles.pontszam} / {solution.ertekeles.osszPontszam}
                                </div>
                            </div>
                            <div className="form-group">
                                <label>Értékelés Dátuma</label>
                                <div className="static-value">{new Date(solution.ertekeles.ertekelesDatuma).toLocaleString()}</div>
                            </div>
                            <div className="form-group">
                                <label>Részletes Visszajelzés</label>
                                <pre className="code-block" style={{ whiteSpace: 'pre-wrap' }}>{solution.ertekeles.llmVisszajelzes}</pre>
                            </div>
                        </>
                    )}
                </div>
            )}

            {!solution.ertekeles && (
                <div className="card" style={{ marginTop: '2rem', backgroundColor: '#fff3cd', borderColor: '#ffc107' }}>
                    <p style={{ margin: 0 }}>Ez a megoldás még nincs értékelve.</p>
                </div>
            )}
        </div>
    );
};

export default SolutionDetails;
