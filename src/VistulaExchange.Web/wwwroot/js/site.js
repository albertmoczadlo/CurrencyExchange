const WATCHLIST_STORAGE_KEY = "ve.watchlist";

document.addEventListener("DOMContentLoaded", () => {
  initialiseConverter();
  initialiseBoardFilters();
  initialiseHistoryCharts();
  initialiseTradeQuotes();
  initialiseAlertEditor();
  initialiseWatchlist();
  initialiseAdminCharts();
  initialisePdfExport();
});

function initialiseConverter() {
  const root = document.querySelector("[data-converter-rates]");
  if (!root) {
    return;
  }

  const rates = safeJsonParse(root.getAttribute("data-converter-rates"), []);
  const amountInput = root.querySelector("[data-converter-amount]");
  const codeSelect = root.querySelector("[data-converter-code]");
  const resultAmount = root.querySelector("[data-converter-result-amount]");
  const resultCode = root.querySelector("[data-converter-result-code]");
  const rateNode = root.querySelector("[data-converter-rate]");
  const spreadNode = root.querySelector("[data-converter-spread]");
  const countryNode = root.querySelector("[data-converter-country]");
  const noteNode = root.querySelector("[data-converter-note]");
  const formatter = createNumberFormatter(2, 4);

  if (!amountInput || !codeSelect || !resultAmount || !resultCode || !rateNode || !spreadNode || !countryNode || !noteNode) {
    return;
  }

  const render = () => {
    const selectedRate = rates.find((rate) => rate.code === codeSelect.value);
    const amount = parseDecimal(amountInput.value);

    if (!selectedRate || selectedRate.buy <= 0) {
      return;
    }

    const receiveAmount = amount / selectedRate.buy;
    resultAmount.textContent = formatter.format(receiveAmount);
    resultCode.textContent = selectedRate.code;
    rateNode.textContent = formatter.format(selectedRate.buy);
    spreadNode.textContent = formatter.format(selectedRate.spread);
    countryNode.textContent = selectedRate.country;
    noteNode.textContent = `Desk buy rate for ${selectedRate.code} is ${formatter.format(selectedRate.buy)} PLN. Sell rate is ${formatter.format(selectedRate.sell)} PLN.`;
  };

  amountInput.addEventListener("input", render);
  codeSelect.addEventListener("change", render);
  render();
}

function initialiseBoardFilters() {
  const searchInputs = document.querySelectorAll("[data-board-filter]");

  searchInputs.forEach((input) => {
    const shell = input.closest("[data-board-shell]");
    const grid = shell?.querySelector("[data-board-grid]");
    const countNode = shell?.querySelector("[data-results-count]");
    const emptyState = shell?.querySelector("[data-empty-state]");

    if (!grid || !countNode || !emptyState) {
      return;
    }

    const cards = Array.from(grid.querySelectorAll("[data-market-card]"));

    const applyFilter = (value) => {
      const query = value.trim().toLowerCase();
      let visibleCards = 0;

      cards.forEach((card) => {
        const haystack = (card.getAttribute("data-market-name") || "").toLowerCase();
        const isVisible = query.length === 0 || haystack.includes(query);
        card.hidden = !isVisible;

        if (isVisible) {
          visibleCards += 1;
        }
      });

      countNode.textContent = String(visibleCards);
      emptyState.hidden = visibleCards !== 0;
    };

    input.addEventListener("input", () => applyFilter(input.value));
    applyFilter(input.value);
  });
}

function initialiseHistoryCharts() {
  const shells = document.querySelectorAll("[data-history-shell]");
  shells.forEach((shell) => {
    const endpoint = shell.getAttribute("data-history-endpoint");
    const canvas = shell.querySelector("[data-history-chart]");
    const codeInput = shell.querySelector("[data-history-code]");
    const buttons = Array.from(shell.querySelectorAll("[data-history-days]"));

    if (!endpoint || !canvas || typeof Chart === "undefined") {
      return;
    }

    let chartInstance = null;
    let selectedDays = Number(buttons.find((button) => button.classList.contains("is-active"))?.getAttribute("data-history-days") || 30);

    const resolveCode = () => {
      if (!codeInput) {
        return shell.getAttribute("data-history-default") || "";
      }

      if (codeInput.tagName === "SELECT" || codeInput.tagName === "INPUT") {
        return codeInput.value || shell.getAttribute("data-history-default") || "";
      }

      return shell.getAttribute("data-history-default") || "";
    };

    const renderChart = async () => {
      const currencyCode = resolveCode();
      if (!currencyCode) {
        return;
      }

      const query = `${endpoint}?currencyCode=${encodeURIComponent(currencyCode)}&days=${selectedDays}`;
      const response = await fetch(query, { headers: { "X-Requested-With": "XMLHttpRequest" } });
      const points = await response.json();
      const labels = points.map((point) => point.date);
      const buyData = points.map((point) => point.buyAt);
      const sellData = points.map((point) => point.sellAt);

      if (chartInstance) {
        chartInstance.destroy();
      }

      chartInstance = new Chart(canvas, {
        type: "line",
        data: {
          labels,
          datasets: [
            {
              label: `${currencyCode} buy`,
              data: buyData,
              borderColor: "#c46a1b",
              backgroundColor: "rgba(196, 106, 27, 0.12)",
              tension: 0.28,
              fill: false
            },
            {
              label: `${currencyCode} sell`,
              data: sellData,
              borderColor: "#0c7a6f",
              backgroundColor: "rgba(12, 122, 111, 0.12)",
              tension: 0.28,
              fill: false
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          interaction: {
            intersect: false,
            mode: "index"
          },
          plugins: {
            legend: {
              labels: {
                color: "#13273a"
              }
            }
          },
          scales: {
            x: {
              ticks: {
                color: "#5c6977"
              },
              grid: {
                color: "rgba(19, 39, 58, 0.08)"
              }
            },
            y: {
              ticks: {
                color: "#5c6977"
              },
              grid: {
                color: "rgba(19, 39, 58, 0.08)"
              }
            }
          }
        }
      });
    };

    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        buttons.forEach((item) => item.classList.remove("is-active"));
        button.classList.add("is-active");
        selectedDays = Number(button.getAttribute("data-history-days") || "30");
        renderChart();
      });
    });

    if (codeInput) {
      codeInput.addEventListener("change", renderChart);
    }

    renderChart();
  });
}

function initialiseTradeQuotes() {
  const shells = document.querySelectorAll("[data-quote-shell]");
  shells.forEach((shell) => {
    const amountInput = shell.querySelector("[data-quote-amount]");
    const rateInput = shell.querySelector("[data-quote-rate]");
    const feeRateInput = shell.querySelector("[data-quote-fee-rate]");
    const grossNode = shell.querySelector("[data-quote-gross]");
    const feeNode = shell.querySelector("[data-quote-fee]");
    const netNode = shell.querySelector("[data-quote-net]");
    const rateNode = shell.querySelector("[data-quote-rate-output]");
    const grossHidden = shell.querySelector("[data-quote-gross-input]");
    const feeHidden = shell.querySelector("[data-quote-fee-input]");
    const netHidden = shell.querySelector("[data-quote-net-input]");
    const submitButton = shell.querySelector("[data-quote-submit]");
    const countdownNode = shell.querySelector("[data-quote-countdown]");
    const formatter = createNumberFormatter(2, 4);
    const mode = shell.getAttribute("data-quote-mode") || "buy";

    if (!amountInput || !rateInput || !feeRateInput || !grossNode || !feeNode || !netNode || !rateNode || !grossHidden || !feeHidden || !netHidden || !submitButton || !countdownNode) {
      return;
    }

    const renderAmounts = () => {
      const rate = parseDecimal(rateInput.value);
      const feeRate = parseDecimal(feeRateInput.value);
      const amount = parseDecimal(amountInput.value);
      const gross = amount * rate;
      const fee = gross * feeRate;
      const net = mode === "sell" ? gross - fee : gross + fee;

      rateNode.textContent = formatter.format(rate);
      grossNode.textContent = formatter.format(gross);
      feeNode.textContent = formatter.format(fee);
      netNode.textContent = formatter.format(Math.max(net, 0));
      grossHidden.value = gross.toFixed(2);
      feeHidden.value = fee.toFixed(2);
      netHidden.value = Math.max(net, 0).toFixed(2);
    };

    const lockedUntil = new Date(shell.getAttribute("data-quote-locked-until") || "");
    const updateCountdown = () => {
      const remainingMs = lockedUntil.getTime() - Date.now();
      if (remainingMs <= 0) {
        shell.classList.add("trade-shell--expired");
        countdownNode.textContent = "Quote expired";
        submitButton.disabled = true;
        return;
      }

      const remainingSeconds = Math.ceil(remainingMs / 1000);
      countdownNode.textContent = `${remainingSeconds}s left`;
    };

    amountInput.addEventListener("input", renderAmounts);
    renderAmounts();
    updateCountdown();
    window.setInterval(updateCountdown, 1000);
  });
}

function initialiseAlertEditor() {
  const editors = document.querySelectorAll("[data-alert-editor]");
  editors.forEach((editor) => {
    const rates = safeJsonParse(editor.getAttribute("data-alert-rates"), []);
    const codeSelect = editor.querySelector("[data-alert-code]");
    const buyInput = editor.querySelector("[data-alert-buy]");
    const sellInput = editor.querySelector("[data-alert-sell]");
    const currentBuyNode = editor.parentElement?.querySelector("[data-alert-current-buy]");
    const currentSellNode = editor.parentElement?.querySelector("[data-alert-current-sell]");
    const formatter = createNumberFormatter(4, 4);

    if (!codeSelect || !buyInput || !sellInput || !currentBuyNode || !currentSellNode) {
      return;
    }

    const getRate = () => rates.find((item) => item.code === codeSelect.value);

    const renderCurrentRate = () => {
      const currentRate = getRate();
      currentBuyNode.textContent = currentRate ? formatter.format(currentRate.buy) : "0.0000";
      currentSellNode.textContent = currentRate ? formatter.format(currentRate.sell) : "0.0000";
    };

    editor.parentElement?.querySelectorAll("[data-alert-preset]").forEach((button) => {
      button.addEventListener("click", () => {
        const currentRate = getRate();
        if (!currentRate) {
          return;
        }

        const percent = Number(button.getAttribute("data-alert-percent") || "0") / 100;
        if (button.getAttribute("data-alert-preset") === "buy") {
          buyInput.value = (currentRate.buy * (1 - percent)).toFixed(4);
        }

        if (button.getAttribute("data-alert-preset") === "sell") {
          sellInput.value = (currentRate.sell * (1 + percent)).toFixed(4);
        }
      });
    });

    codeSelect.addEventListener("change", renderCurrentRate);
    renderCurrentRate();
  });
}

function initialiseWatchlist() {
  const buttons = document.querySelectorAll("[data-watchlist-toggle]");

  buttons.forEach((button) => {
    const code = button.getAttribute("data-watchlist-code");
    if (!code) {
      return;
    }

    renderWatchlistButton(button, code);
    button.addEventListener("click", () => {
      toggleWatchlist(code);
      document.querySelectorAll(`[data-watchlist-code="${code}"]`).forEach((duplicate) => renderWatchlistButton(duplicate, code));
      renderWatchlistPanels();
    });
  });

  renderWatchlistPanels();
}

function initialiseAdminCharts() {
  const roots = document.querySelectorAll("[data-admin-dashboard]");
  roots.forEach((root) => {
    if (typeof Chart === "undefined") {
      return;
    }

    const activityData = safeJsonParse(root.getAttribute("data-admin-activity"), []);
    const turnoverData = safeJsonParse(root.getAttribute("data-admin-turnover"), []);
    const activityCanvas = root.querySelector("[data-admin-activity-chart]");
    const turnoverCanvas = root.querySelector("[data-admin-turnover-chart]");

    if (activityCanvas) {
      new Chart(activityCanvas, {
        type: "bar",
        data: {
          labels: activityData.map((point) => point.label),
          datasets: [
            {
              label: "Transactions",
              data: activityData.map((point) => point.value),
              backgroundColor: "#0c7a6f",
              borderRadius: 10
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { display: false }
          },
          scales: {
            x: {
              ticks: { color: "#5c6977" },
              grid: { display: false }
            },
            y: {
              ticks: { color: "#5c6977" },
              grid: { color: "rgba(19, 39, 58, 0.08)" }
            }
          }
        }
      });
    }

    if (turnoverCanvas) {
      new Chart(turnoverCanvas, {
        type: "doughnut",
        data: {
          labels: turnoverData.map((item) => item.code),
          datasets: [
            {
              data: turnoverData.map((item) => item.value),
              backgroundColor: ["#0c7a6f", "#c46a1b", "#13273a", "#d59d49", "#7b9d87", "#df8a68"]
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: "bottom",
              labels: { color: "#13273a" }
            }
          }
        }
      });
    }
  });
}

function initialisePdfExport() {
  document.querySelectorAll("form").forEach((form) => {
    const gridInput = form.querySelector("input[name='GridHtml']");
    if (!gridInput) {
      return;
    }

    form.addEventListener("submit", () => {
      const grid = document.getElementById("Grid");
      if (grid) {
        gridInput.value = grid.innerHTML;
      }
    });
  });
}

function renderWatchlistPanels() {
  document.querySelectorAll("[data-watchlist-root]").forEach((root) => {
    const shell = root.closest("[data-watchlist-shell]");
    const quotes = safeJsonParse(shell?.getAttribute("data-market-quotes"), []);
    const watchlist = getWatchlist();
    const items = quotes.filter((quote) => watchlist.includes(quote.code));

    if (items.length === 0) {
      root.innerHTML = '<p class="dashboard-card__empty-copy">Use the star buttons on the live board to pin your market watchlist here.</p>';
      return;
    }

    root.innerHTML = items.map((item) => `
      <article class="watchlist-card">
        <div class="watchlist-card__head">
          <strong>${item.code}</strong>
          <button type="button" class="watchlist-toggle is-active" data-watchlist-toggle data-watchlist-code="${item.code}" aria-label="Remove ${item.code} from watchlist">
            <i class="ri-star-fill"></i>
          </button>
        </div>
        <p>${item.name}</p>
        <dl class="watchlist-card__stats">
          <div><dt>Buy</dt><dd>${formatNumber(item.buy, 4)}</dd></div>
          <div><dt>Sell</dt><dd>${formatNumber(item.sell, 4)}</dd></div>
        </dl>
      </article>
    `).join("");

    root.querySelectorAll("[data-watchlist-toggle]").forEach((button) => {
      const code = button.getAttribute("data-watchlist-code");
      if (!code) {
        return;
      }

      button.addEventListener("click", () => {
        toggleWatchlist(code);
        document.querySelectorAll(`[data-watchlist-code="${code}"]`).forEach((duplicate) => renderWatchlistButton(duplicate, code));
        renderWatchlistPanels();
      });
    });
  });
}

function renderWatchlistButton(button, code) {
  const isActive = getWatchlist().includes(code);
  button.classList.toggle("is-active", isActive);
  button.innerHTML = `<i class="${isActive ? "ri-star-fill" : "ri-star-line"}"></i>`;
}

function getWatchlist() {
  try {
    const raw = window.localStorage.getItem(WATCHLIST_STORAGE_KEY);
    if (!raw) {
      return [];
    }

    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

function toggleWatchlist(code) {
  const current = new Set(getWatchlist());
  if (current.has(code)) {
    current.delete(code);
  } else {
    current.add(code);
  }

  window.localStorage.setItem(WATCHLIST_STORAGE_KEY, JSON.stringify(Array.from(current)));
}

function safeJsonParse(rawValue, fallbackValue) {
  if (!rawValue) {
    return fallbackValue;
  }

  try {
    return JSON.parse(rawValue);
  } catch {
    return fallbackValue;
  }
}

function parseDecimal(value) {
  return Number.parseFloat(String(value || "0").replace(",", ".")) || 0;
}

function createNumberFormatter(minimumFractionDigits, maximumFractionDigits) {
  return new Intl.NumberFormat("pl-PL", {
    minimumFractionDigits,
    maximumFractionDigits
  });
}

function formatNumber(value, fractionDigits) {
  return createNumberFormatter(fractionDigits, fractionDigits).format(value || 0);
}
