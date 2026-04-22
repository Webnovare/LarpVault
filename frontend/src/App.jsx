import { useState, useEffect } from 'react';

const API_BASE = 'https://larpvault-functions-eadfdghpdqbgexah.eastus-01.azurewebsites.net/api';

function App() {
  const [packs, setPacks] = useState([]);
  const [email, setEmail] = useState('adam@test.com');
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchPacks();
  }, []);

  const fetchPacks = async () => {
    try {
      const res = await fetch(`${API_BASE}/packs`);
      const data = await res.json();
      if (data.packs) setPacks(data.packs);
    } catch (err) {
      console.error(err);
    }
  };

  const handlePurchase = async (packName) => {
    setLoading(true);
    setResult(null);

    try {
      const res = await fetch(`${API_BASE}/purchase`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, packName, price: 10000 })
      });

      const data = await res.json();

      if (res.ok) {
        setResult({
          success: true,
          orderId: data.orderId,
          message: data.message || 'Your LARP pack is being processed in the background.'
        });
      } else {
        throw new Error(data.error || 'Purchase failed');
      }
    } catch (err) {
      setResult({ success: false, message: 'Cannot connect to backend — retrying...' });
      setTimeout(() => handlePurchase(packName), 2000);
    } finally {
      setLoading(false);
    }
  };

  const copyOrderId = (id) => {
    navigator.clipboard.writeText(id);
    alert('✅ Order ID copied!');
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-950 via-slate-900 to-black text-white p-6">
      <div className="max-w-2xl mx-auto">
        {/* Header */}
        <div className="text-center mb-12">
          <h1 className="text-7xl font-black tracking-tighter mb-2">🏴‍☠️ LarpVault</h1>
          <p className="text-4xl text-amber-400 font-light">Live Like A Legend</p>
          <p className="text-gray-400 mt-2 text-lg">Only $10,000 for perceived success</p>
        </div>

        {/* Buy Card */}
        <div className="glass-card rounded-3xl p-8 mb-10">
          <h2 className="text-3xl font-semibold mb-6 text-center">Buy Your Pack</h2>
          <div>
            <label className="block text-sm font-medium mb-3 text-gray-300">Your Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full px-6 py-4 bg-white/10 border border-white/30 rounded-2xl focus:outline-none focus:border-amber-400 text-lg placeholder-gray-400"
            />
          </div>
        </div>

        {/* Packs */}
        <div className="glass-card rounded-3xl p-8">
          <h2 className="text-3xl font-semibold mb-8">📦 Available LARP Packs</h2>
          <div className="space-y-6">
            {packs.map((pack) => (
              <div
                key={pack.name}
                className="glass-card p-6 rounded-3xl hover:scale-[1.03] transition-all duration-300 border border-white/10 hover:border-amber-400"
              >
                <div className="flex justify-between items-start">
                  <div className="flex-1 pr-6">
                    <h3 className="text-2xl font-semibold">{pack.name}</h3>
                    <p className="text-gray-400 mt-3 leading-relaxed">{pack.description}</p>
                  </div>
                  <div className="text-right min-w-[140px]">
                    <p className="text-5xl font-bold text-amber-400">${pack.price.toLocaleString()}</p>
                    <p className="text-xs uppercase tracking-widest text-gray-500 mt-1">{pack.clips} clips</p>
                    <button
                      onClick={() => handlePurchase(pack.name)}
                      disabled={loading}
                      className="mt-6 w-full bg-amber-400 hover:bg-amber-500 disabled:bg-gray-600 text-black font-bold py-4 rounded-2xl transition-colors text-lg"
                    >
                      {loading ? 'Processing...' : 'Buy This Pack'}
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Result */}
        {result && (
          <div className={`mt-10 p-8 rounded-3xl ${result.success ? 'bg-emerald-900/40 border border-emerald-400' : 'bg-red-900/40 border border-red-400'}`}>
            {result.success ? (
              <>
                <p className="text-3xl mb-4">🎟️ Purchase Successful!</p>
                <p className="mb-4">Order ID: <span className="font-mono bg-black/50 px-4 py-2 rounded-2xl">{result.orderId}</span></p>
                <p className="text-emerald-200 text-lg">{result.message}</p>
                <button
                  onClick={() => copyOrderId(result.orderId)}
                  className="mt-6 px-8 py-4 bg-white/20 hover:bg-white/30 rounded-2xl text-sm font-medium"
                >
                  📋 Copy Order ID
                </button>
              </>
            ) : (
              <p className="text-red-300">{result.message}</p>
            )}
          </div>
        )}
      </div>
    </div>
  );
}

export default App;