import apiClient from '../config/axios.js'
export const motorbikeService = {
  async getAll(query) {
    try{
        const response = await apiClient.get('/Motorbike', { params: query })
        return response.data
    }catch(error) {
      console.log(error)
      throw error
    }
  },
  async getFilterOptions() {
    try {
      const response = await apiClient.get('Motorbike/filter-options')
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async getById(id) {
    try {
      const response = await apiClient.get('/Motorbike/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw log
    }
  },
  async createMotorbike(form) {
    const formData = new FormData()
    formData.append('motorbikeId', 0)
    formData.append('motorbikeName', form.motorbikeName)
    formData.append('categoryId', form.categoryId)
    formData.append('hourlyRate', form.hourlyRate)
    formData.append('dailyRate', form.dailyRate)
    formData.append('licensePlate', form.licensePlate)
    formData.append('brand', form.brand)
    formData.append('year', form.year)
    formData.append('color', form.color)
    formData.append('engineCapacity', form.engineCapacity)
    formData.append('chassisNumber', form.chassisNumber)
    formData.append('engineNumber', form.engineNumber)
    formData.append('description', form.description)
    formData.append('motorbikeConditionStatus', form.motorbikeConditionStatus)
    formData.append('imageUrl', form.imageUrl)
    formData.append('mileage', form.mileage)
    formData.append('status', form.status)
    if (form.formFile) {
      formData.append('formFile', form.formFile)
    }
    try {
      const response = await apiClient.post('/Motorbike', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async updateMotorbike(form) {
    const formData = new FormData()
    formData.append('motorbikeName', form.motorbikeName)
    formData.append('categoryId', form.categoryId)
    formData.append('hourlyRate', form.hourlyRate)
    formData.append('dailyRate', form.dailyRate)
    formData.append('licensePlate', form.licensePlate)
    formData.append('brand', form.brand)
    formData.append('year', form.year)
    formData.append('color', form.color)
    formData.append('engineCapacity', form.engineCapacity)
    formData.append('chassisNumber', form.chassisNumber)
    formData.append('engineNumber', form.engineNumber)
    formData.append('description', form.description)
    formData.append('motorbikeConditionStatus', form.motorbikeConditionStatus)
    if (form.formFile) {
      formData.append('formFile', form.formFile)
    }
    try {
      const response = await apiClient.put('/Motorbike/' + form.motorbikeId, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
  async deleteMotorbike(id) {
    try {
      const response = await apiClient.delete('/Motorbike/' + id)
      return response.data
    } catch (error) {
      console.log(error)
      throw error
    }
  },
}
