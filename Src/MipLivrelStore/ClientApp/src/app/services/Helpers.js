import { decode } from 'jsonwebtoken';
export const checkToken = (history, token) => {
 
    if (decode(token).exp * 1000 < Date.now()) {
        localStorage.clear();
        history.push('/')
    }
}