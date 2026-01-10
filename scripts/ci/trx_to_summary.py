import glob, json, sys
import xml.etree.ElementTree as ET
from datetime import datetime, timezone

trx_files = glob.glob("**/*.trx", recursive=True)
total = passed = failed = 0

for f in trx_files:
    root = ET.parse(f).getroot()
    ns = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
    counters = root.find(".//t:ResultSummary/t:Counters", ns)
    if counters is None:
        continue
    total += int(counters.attrib.get("total", 0))
    passed += int(counters.attrib.get("passed", 0))
    failed += int(counters.attrib.get("failed", 0))

out = {
    "pipeline": "github-actions",
    "total": total,
    "passed": passed,
    "failed": failed,
    "generatedAtUtc": datetime.now(timezone.utc).isoformat()
}
print(json.dumps(out, indent=2))
