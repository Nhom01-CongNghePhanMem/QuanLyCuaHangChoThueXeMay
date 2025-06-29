import apiClient from "../config/axios";

export const employeeService = {
    async getAll(query) {
        try{
            const response = await apiClient.get('/Employee', { params: query });
            return response.data;
        }catch (error) {
            console.log(error);
            throw error;
        }
    },
    
}