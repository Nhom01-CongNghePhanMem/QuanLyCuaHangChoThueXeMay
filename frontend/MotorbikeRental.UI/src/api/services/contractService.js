import apiClient from "../config/axios";
export const contractService = {
    async getAll(query) {
        try{
            const response = await apiClient.get('/Contract/GetContractsByFilter', { params: query });
            return response.data;
        }catch (error) {
            console.log(error);
            throw error;
        }
    },
    async calculateContractPrice(form) {
        try {
            const response = await apiClient.post('/Contract/calculate-price', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async createContract(form) {
        try {
            const response = await apiClient.post('/Contract', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async updateContractStatusActive(contractId) {
        try {
            const response = await apiClient.post('/Contract/UpdateContractStatusActive', contractId);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async cancelContractByCustomer(contractId) {
        try {
            const response = await apiClient.post('/Contract/CancelContractByCustomer', contractId);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async updateContractBeforeActivation(form) {
        try {
            const response = await apiClient.put('/Contract/update-before-activation', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async getById(contractId) {
        try{
            const response = await apiClient.get('/Contract/' + contractId + '/GetContractById');
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async deleteContract(contractId) {
        try {
            await apiClient.delete('/Contract/' + contractId + '/DeleteContract');
        } catch (error) {
            console.log(error);
            throw error;
        }
    },
    async contractSettlement(form) {
        try {
            const response = await apiClient.post('/Contract/ContractSettlement', form);
            return response.data;
        } catch (error) {
            console.log(error);
            throw error;
        }
    }
}