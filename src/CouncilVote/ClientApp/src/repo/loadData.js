const post = async (url, body) => {
    return fetch(url, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

const get = async (url) => {
    return fetch(url, {
        method: 'GET'
    });
}

const createMeasure = async body => {
  return await post('/api/measure', body)
      .then(res => res.json());
}

const getMeasure = async id => {
    return await get(`/api/measure/${id}`)
        .then(res => res.json());
}

export { createMeasure, getMeasure };