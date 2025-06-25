import apiClient from '../config/axios.js'
export const categoryService = {
  async getAll() {
    try {
      const response = await apiClient.get('/Category')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
}
