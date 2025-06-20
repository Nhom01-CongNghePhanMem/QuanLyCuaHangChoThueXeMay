import apiClient from '../config/axios.js'
export const motorbikeService = {
  async getAll() {
    try {
      const response = await apiClient.get('/Motorbike')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
}
