import apiClient from '../config/axios'
export const userCredentialsService = {
  async getById(employeeId) {
    try {
      const response = await apiClient.get('/UserCredentials/' + employeeId + '/GetUserCredentials')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editUserName(employeeId, form) {
    try{
        const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-userName', form)
        return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editEmail(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-email', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editPhoneNumber(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-phoneNumber', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editPassword(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-passwordByAdmin', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async editRole(employeeId, form) {
    try {
      const response = await apiClient.post('/UserCredentials/' + employeeId + '/reset-role', form)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteUserCredentials(employeeId) {
    try {
      await apiClient.delete('/UserCredentials/' + employeeId + '/deleteUserCredentialsByAdmin')
    } catch (error) {
      console.log(error)
      throw error
    }
  }
}
