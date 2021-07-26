const post = async (url, body) => {
    return fetch(url, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

const createMeasure = async body => {
  return await post('/api/measure', body)
      .then(res => res.json());
}

export { createMeasure };