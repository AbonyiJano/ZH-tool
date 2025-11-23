export const API_BASE_URL = 'http://localhost:5066/api';

export const ENDPOINTS = {
    STUDENT: {
        LOGIN: '/Hallgato', // POST { Neptunkod, Nev }
    },
    EXAM: {
        LIST: '/Zh', // GET
        LIST_GENERATED: '/Zh/generated', // GET
        GET_BY_ID: (id) => `/Zh/${id}`, // GET
        CREATE: '/Zh', // POST
        GENERATE: (parentZhId) => `/Zh/${parentZhId}/generate`, // POST
    },
    TASK: {
        GET_BY_GENERATED_ZH: (generaltZhId) => `/Feladat/bygeneraltzh/${generaltZhId}`, // GET
        UPDATE: (id) => `/Feladat/${id}`, // PUT
    },
    SOLUTION: {
        SUBMIT: '/Megoldas/submit-megoldas', // POST { HallgatoNeptunkod, GeneraltZhId, BekuldottMegoldas }
        GET_BY_ID: (id) => `/Megoldas/megoldas/${id}`, // GET
    }
};
