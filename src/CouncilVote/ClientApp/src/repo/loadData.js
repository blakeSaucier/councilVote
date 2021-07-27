const post = async (url, body) =>
    fetch(url, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json'
        }
    })

const get = async (url) =>
    fetch(url, {
        method: 'GET'
    })

const createMeasure = async body =>
    await post('/api/measure', body)
        .then(res => res.json())

const getMeasure = async id =>
    await get(`/api/measure/${id}`)
        .then(res => res.json())

const submitVote = async vote =>
    await post('/api/vote', vote)
        .then(res => res.json())

export { createMeasure, getMeasure, submitVote };