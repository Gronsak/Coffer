async function fetchLogin(email, password, remeberme){
    try {
        const res = await fetch("/api/auth/login", {
            method: "Post",
            headers: { "Content-Type": "application/json" },
            credentials: "same-origin",
            body: JSON.stringify({ email, password, remeberme })
        });

        const ct = res.headers.get("content-type") || "";
        const body = ct.includes("application/json") ? await res.json() : await res.text();

        return { ok: res.ok, status: res.status, body };
    } catch (err) {
        return { ok: false, status: 0, error: err?.message ?? String(err) };
    }
};