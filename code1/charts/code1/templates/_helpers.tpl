{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "code1.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "code1.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "code1.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "code1.labels" -}}
helm.sh/chart: {{ include "code1.chart" . }}
{{ include "code1.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "code1.selectorLabels" -}}
app.kubernetes.io/name: {{ include "code1.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "code1.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "code1.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Define the code1.basePath template with
= /code1
*/}}
{{- define "code1.basePath" -}}
{{- if .Values.basePath -}}
{{ printf "%s" .Values.basePath }}
{{- else -}}
{{ printf "" }}
{{- end -}}
{{- end -}}

{{/*
Define the code1.basePathPreSlash template with basePath and Slash
= /code1
*/}}
{{- define "code1.basePathPreSlash" -}}
{{- if .Values.basePath -}}
{{ printf "/%s" .Values.basePath }}
{{- else -}}
{{ printf "" }}
{{- end -}}
{{- end -}}


{{/*
Define the code1.basePathPostSlash template with basePath and Slash
= code1/
*/}}
{{- define "code1.basePathPostSlash" -}}
{{- if .Values.basePath -}}
{{ printf "%s/" .Values.basePath }}
{{- else -}}
{{ printf "" }}
{{- end -}}
{{- end -}}

