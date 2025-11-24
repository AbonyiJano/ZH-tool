export const API_BASE_URL = 'http://localhost:5066/api';

export const ENDPOINTS = {
    STUDENT: {
        LOGIN: '/Hallgato', // POST { Neptunkod, Nev }
        LIST: '/Hallgato', // GET
    },
    EXAM: {
        LIST: '/Zh', // GET
        LIST_GENERATED: '/Zh/generalt', // GET
        GET_BY_ID: (id) => `/Zh/${id}`, // GET
        CREATE: '/Zh', // POST
        GENERATE: (parentZhId) => `/Zh/${parentZhId}/generalas`, // POST
    },
    TASK: {
        GET_BY_GENERATED_ZH: (generaltZhId) => `/Feladat/generaltzh/${generaltZhId}`, // GET
        UPDATE: (id) => `/Feladat/${id}`, // PUT
    },
    SOLUTION: {
        SUBMIT: '/Megoldas/bekuldes', // POST { HallgatoNeptunkod, GeneraltZhId, BekuldottMegoldas }
        GET_BY_ID: (id) => `/Megoldas/${id}`, // GET
        LIST: '/Megoldas', // GET
    },
    EVALUATION: {
        UPDATE: (id) => `/Ertekeles/${id}`, // PUT
    }
};
